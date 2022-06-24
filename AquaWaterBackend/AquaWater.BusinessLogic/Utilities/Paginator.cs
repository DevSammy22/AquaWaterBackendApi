using AquaWater.Domain.Commons;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AquaWater.BusinessLogic.Utilities
{
    public static class Paginator
    {
        public static SearchResponse<IEnumerable<TSource>> Pagination<TSource>
                                (IEnumerable<TSource> queryable, int pageSize, int pageNumber)
        {
            var count = queryable.Count();
            var pageResult = new SearchResponse<IEnumerable<TSource>>
            {
                PageSize = (pageSize > 10 || pageSize < 1) ? 10 : pageSize,
                CurrentPage = pageNumber >= 1 ? pageNumber : 1,
                PreviousPage = pageNumber > 0 ? pageNumber - 1 : 0

            };
            pageResult.TotalNumberOfPages = (int)Math.Ceiling(count / (double)(pageResult.PageSize));

            var sourceList = queryable.Skip(pageResult.CurrentPage * pageResult.PageSize).Take(pageResult.PageSize).ToList();
            pageResult.PageItems = sourceList;
            return pageResult;
        }

        public static SearchResponse<IEnumerable<TResponse>> Pagination<TSource, TResponse>
                               (IEnumerable<TSource> queryable, IMapper mapper, int pageSize, int pageNumber)
        {
            var count = queryable.Count();
            var pageResult = new SearchResponse<IEnumerable<TResponse>>
            {
                PageSize = (pageSize > 10 || pageSize < 1) ? 10 : pageSize,
                CurrentPage = pageNumber >= 1 ? pageNumber : 1,
                PreviousPage = pageNumber > 0 ? pageNumber - 1 : 0
            };
            pageResult.TotalNumberOfPages = (int)Math.Ceiling(count / (double)(pageResult.PageSize));

            var sourceList = queryable.Skip((pageResult.CurrentPage - 1) * pageResult.PageSize).Take(pageResult.PageSize).ToList();
            pageResult.PageItems = mapper.Map<IList<TResponse>>(sourceList);
            return pageResult;
        }


        
    }
}