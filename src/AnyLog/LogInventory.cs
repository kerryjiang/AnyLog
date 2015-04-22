using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnyLog
{
    public interface ILogInventory
    {
        ILog CreateLogFromRepository(string repositoryName, string name);
    }

    public class LogInventory<T> : ILogInventory
        where T : class
    {
        private ConcurrentDictionary<string, Lazy<T>> m_LoggerRepositories = new ConcurrentDictionary<string, Lazy<T>>();

        private Func<string, string> m_RepositoryConfigGetter;
        private Func<string, string, T> m_RepositoryCreator;
        private Func<T, string, ILog> m_LogCreator;

        public LogInventory(Func<string, string> repositoryConfigGetter, Func<string, string, T> repositoryCreator, Func<T, string, ILog> logCreator)
        {
            m_RepositoryConfigGetter = repositoryConfigGetter;
            m_RepositoryCreator = repositoryCreator;
            m_LogCreator = logCreator;
        }

        private Lazy<T> CreateLazyRepository(string repositoryName)
        {
            var configFilePath = m_RepositoryConfigGetter(repositoryName);

            if (!File.Exists(configFilePath))
                return null;

            return new Lazy<T>(() =>
            {
                return m_RepositoryCreator(repositoryName, configFilePath);
            });
        }

        public T EnsureRepository(string repositoryName)
        {
            Lazy<T> repository;

            if (m_LoggerRepositories.TryGetValue(repositoryName, out repository))
                return repository.Value;

            repository = CreateLazyRepository(repositoryName);

            if (repository == null)
                return null;

            if (m_LoggerRepositories.TryAdd(repositoryName, repository))
                return repository.Value;

            return EnsureRepository(repositoryName);
        }

        public ILog CreateLogFromRepository(string repositoryName, string name)
        {
            // try to create log
            // get log respostory at first
            T repository = EnsureRepository(repositoryName);

            // repository is not found
            if (repository == null)
                return null;

            return m_LogCreator(repository, name);
        }
    }
}
