using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;

namespace Infrastructure.MediaData
{
    public class MediaRepository : IMediaRepository
    {
        private readonly MainDBContext _db;
        public MediaRepository(MainDBContext db) {
            _db = db;
        }

        public async Task<Media> CreateMedia(Media media, CancellationToken cancellationToken)
        {
            await _db.Medias.AddAsync(media, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return media;
        }
    }
}
