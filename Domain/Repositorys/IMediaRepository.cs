﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositorys
{
    public interface IMediaRepository
    {
        Task<Media> CreateMediaAsync (Media media, CancellationToken cancellationToken);
    }
}
