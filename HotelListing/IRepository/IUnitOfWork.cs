using HotelListing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Country{ get; }
        Task Save();


    }
}
