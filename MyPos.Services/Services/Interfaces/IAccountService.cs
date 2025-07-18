﻿using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAll(int pageNumber, int pageSize, Func<IQueryable<AccountEntity>, IQueryable<AccountEntity>>? filter = null);

        Task<AccountDto> CreateAccount(CreateAccountDto dto);

        Task<AccountDto> GetById(int id);

        Task<AccountDto> UpdateAccount(int id, CreateAccountDto dto);
        Task<AccountDto> DeleteAccount(int id);
    }
}
