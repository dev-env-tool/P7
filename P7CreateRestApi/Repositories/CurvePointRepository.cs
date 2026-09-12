using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using System.Collections;


namespace P7CreateRestApi.Repositories
{
    public class CurvePointRepository : ICurvePointRepository
    {

        private static P7Referential? _context;

        public CurvePointRepository(P7Referential context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CurvePoint>> GetAllCurvePoints()
        {
            return await _context!.CurvePoints.ToListAsync();
        }

        public async Task<IEnumerable<CurvePoint>> GetCurvePointById(int id)
        {
            return await _context!.CurvePoints.Where(c => c.Id == id)
                                  .ToListAsync();
        }

        public async Task CreateCurvePoint(CurvePoint curvePoint)
        {
            if (curvePoint != null)
            {
                await _context!.CurvePoints.AddAsync(curvePoint);
                await _context!.SaveChangesAsync();
            }
        }

        public async Task UpdateCurvePoint(CurvePoint curvePoint)
        {
            var existingCurvePoint = await _context.CurvePoints.FindAsync(curvePoint.Id);

            if (existingCurvePoint != null)
            {
                _context.Entry(existingCurvePoint).CurrentValues.SetValues(curvePoint);
                await _context!.SaveChangesAsync();
            }
        }


        public async Task DeleteCurvePointById(int id)
        {
            CurvePoint curvePoint = _context!.CurvePoints.First(r => r.Id == id);

            if (curvePoint != null)
            {
                _context!.CurvePoints.Remove(curvePoint);
                await _context.SaveChangesAsync();
            }
        }
    }
}