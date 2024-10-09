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
            var cachedSiswa = await _redisCacheService.GetCacheAsync<List<Siswa>>(SiswaCacheKey);
            if (cachedSiswa != null)
            {
                return Ok(cachedSiswa);
            }

            var siswaList = await _context.Siswas.ToListAsync();
            await _redisCacheService.SetCacheAsync(SiswaCacheKey, siswaList, TimeSpan.FromMinutes(10));

            return Ok(siswaList);
        }

        // GET: api/siswa/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSiswaById(int id)
        {
            var siswa = await _context.Siswas.FindAsync(id);
            if (siswa == null)
            {
                return NotFound();
            }

            return Ok(siswa);
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

        // PUT: api/siswa/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSiswa(int id, PostSiswaDto siswaDto)
        {
            var siswa = await _context.Siswas.FindAsync(id);
            if (siswa == null)
            {
                return NotFound();
            }

            siswa.Nama = siswaDto.Nama;
            siswa.Kelas = siswaDto.Kelas;
            siswa.Umur = siswaDto.Umur;
            siswa.WaliKelas = siswaDto.WaliKelas;
            siswa.AsalKota = siswaDto.AsalKota;

            _context.Entry(siswa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var siswaList = await _context.Siswas.ToListAsync();
            await _redisCacheService.SetCacheAsync(SiswaCacheKey, siswaList, TimeSpan.FromMinutes(10));

            return NoContent();
        }

        // DELETE: api/siswa/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSiswa(int id)
        {
            var siswa = await _context.Siswas.FindAsync(id);
            if (siswa == null)
            {
                return NotFound();
            }

            _context.Siswas.Remove(siswa);
            await _context.SaveChangesAsync();

            var siswaList = await _context.Siswas.ToListAsync();
            await _redisCacheService.SetCacheAsync(SiswaCacheKey, siswaList, TimeSpan.FromMinutes(10));

            return NoContent();
        }
    }
}
