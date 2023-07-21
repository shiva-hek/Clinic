﻿using Application.Services;

namespace Infrastructure.Services
{
    public class IdService : IIdService
    {
        public Guid GenerateNewId()
        {
            return Guid.NewGuid();
        }
    }
}