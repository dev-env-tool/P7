using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;


namespace P7CreateRestApi.Services
{
    public class CurvePointService : ICurvePointService
    {
        private readonly ICurvePointRepository _iCurvePointRepository;
        private readonly IMapper _mapper;


        public CurvePointService(ICurvePointRepository iCurvePointRepository, IMapper iMapper)
        {
            _iCurvePointRepository = iCurvePointRepository;
            _mapper = iMapper;
        }
        public async Task<IEnumerable<CurvePointDto>> GetAllCurvePointsDto()
        {
            Task<IEnumerable<CurvePoint>> curvePoints = _iCurvePointRepository!.GetAllCurvePoints();
            List<CurvePointDto> listOfCurvePointDtos = new List<CurvePointDto>();
            foreach (CurvePoint curvePoint in await curvePoints)
            {
                CurvePointDto curvePointDto = await MapCurvePointToCurvePointDto(curvePoint);
                listOfCurvePointDtos.Add(curvePointDto);
            }

            return listOfCurvePointDtos;
        }

        public async Task<IEnumerable<CurvePointDto>> GetCurvePointDtoById(int id)
        {
            Task<IEnumerable<CurvePoint>> curvePoints = _iCurvePointRepository!.GetCurvePointById(id);
            List<CurvePointDto> listOfCurvePointDtos = new List<CurvePointDto>();
            foreach (CurvePoint curvePoint in await curvePoints)
            {
                CurvePointDto curvePointDto = await MapCurvePointToCurvePointDto(curvePoint);
                listOfCurvePointDtos.Add(curvePointDto);
            }

            return listOfCurvePointDtos;
        }

        public async Task CreateCurvePointWithCurvePointDto(CurvePointDto curvePointDto)
        {
            CurvePoint curvePoint = await MapCurvePointDtoToCurvePoint(curvePointDto);
            if (curvePointDto != null)
            {
                await _iCurvePointRepository.CreateCurvePoint(curvePoint);
            }
        }

        public async Task UpdateCurvePointWithCurvePointDto(CurvePointDto curvePointDto)
        {
            CurvePoint curvePoint = await MapCurvePointDtoToCurvePoint(curvePointDto);
            if (curvePointDto != null)
            {
                await _iCurvePointRepository.UpdateCurvePoint(curvePoint);
            }
        }


        public async Task DeleteCurvePointById(int id)
        {
            await _iCurvePointRepository.DeleteCurvePointById(id);
        }

        public async Task<CurvePointDto> MapCurvePointToCurvePointDto(CurvePoint curvePoint)
        {
            CurvePointDto curvePointDto = _mapper.Map<CurvePoint, CurvePointDto>(curvePoint);
            return curvePointDto;
        }
        public async Task<CurvePoint> MapCurvePointDtoToCurvePoint(CurvePointDto curvePointDto)
        {
            CurvePoint curvePoint = _mapper.Map<CurvePointDto, CurvePoint>(curvePointDto);
            return curvePoint;
        }



    }
}
