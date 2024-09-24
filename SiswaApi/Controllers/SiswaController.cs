using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiswaApi.Data;
using SiswaApi.Dto;
using SiswaApi.Models;

namespace SiswaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiswaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RedisCacheService _redisCacheService;
        private const string SiswaCacheKey = "siswa_list";

        public SiswaController(ApplicationDbContext context, RedisCacheService redisCacheService)
        {
            _context = context;
            _redisCacheService = redisCacheService;
        }

        // GET: api/siswa
        [HttpGet]
        public async Task<IActionResult> GetSiswa()
        {
            // Cek apakah data ada di cache Redis
            var cachedSiswa = await _redisCacheService.GetCacheAsync<List<Siswa>>(SiswaCacheKey);
            if (cachedSiswa != null)
            {
                return Ok(cachedSiswa);
            }

            // Jika tidak ada di cache, ambil dari database
            var siswaList = await _context.Siswas.ToListAsync();

            // Simpan ke Redis cache
            await _redisCacheService.SetCacheAsync(SiswaCacheKey, siswaList, TimeSpan.FromMinutes(10));

            return Ok(siswaList);
        }

        // POST: api/siswa
        [HttpPost]
        public async Task<IActionResult> PostSiswa(PostSiswaDto siswaDto)
        {
            var siswa = new Siswa
            {
                Nama = siswaDto.Nama,
                Kelas = siswaDto.Kelas,
                Umur = siswaDto.Umur,
                WaliKelas = siswaDto.WaliKelas,
                AsalKota = siswaDto.AsalKota
            };

            _context.Siswas.Add(siswa);
            await _context.SaveChangesAsync();

            var siswaList = await _context.Siswas.ToListAsync();

            await _redisCacheService.SetCacheAsync(SiswaCacheKey, siswaList, TimeSpan.FromMinutes(10));

            return CreatedAtAction(nameof(GetSiswa), new { id = siswa.Id }, siswa);
        }
    }
}
