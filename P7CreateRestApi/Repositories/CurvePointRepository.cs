
//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.IRepositories;

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

        public void CreateCurvePoint(CurvePoint curvePoint)
        {
            if (curvePoint != null)
            {
                _context!.CurvePoints.Add(curvePoint);
                _context.SaveChanges();
            }
        }
        public async Task UpdateCurvePoint(CurvePoint curvePoint)
        {
            if (curvePoint != null)
            {
                _context!.Entry(curvePoint).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteCurvePointById(int id)
        {
            CurvePoint CurvePoint = _context!.CurvePoints.First(c => c.Id == id);

            if (CurvePoint != null)
            {
                _context!.CurvePoints.Remove(CurvePoint);
                _context.SaveChanges();
            }
        }
    }
}