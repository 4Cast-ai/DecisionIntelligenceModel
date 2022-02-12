﻿using Infrastructure.Interfaces;
using Model.Entities;

namespace Infrastructure.DataContext
{
    public class UnitOfWorkPoolOptionsBuilder
    {
        public UnitOfWorkPoolOptions Options { get; } = new UnitOfWorkPoolOptions();

        /// <summary>
        /// Registers the name by which a Unit of Work for the given DbContext will be retrievable
        /// </summary>
        /// <typeparam name="T">DbContext for which a Unit of Work will be added</typeparam>
        /// <param name="key">The key by which a Unit of Work for the DbContext will be retrievable in client code</param>
        public void AddUnitOfWork<T>(string key) where T : Context
        {
            Options.RegisteredUoWs.Add(key, typeof(IUnitOfWork<T>));
        }
    }

}
