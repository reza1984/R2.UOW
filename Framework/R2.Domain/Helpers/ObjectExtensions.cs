using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using R2.Domain.Entity.Auditing;

namespace R2.Domain.Helpers
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="pageIndex"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="indexFrom">The start index value.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int indexFrom = 0) => new PagedList<T>(source, pageIndex, pageSize, indexFrom);

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="converter"/>, <paramref name="pageIndex"/> and <paramref name="pageSize"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="source">The source to convert.</param>
        /// <param name="converter">The converter to change the <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="indexFrom">The start index value.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static IPagedList<TResult> ToPagedList<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex, int pageSize, int indexFrom = 0) => new PagedList<TSource, TResult>(source, converter, pageIndex, pageSize, indexFrom);

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="pageIndex"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <param name="indexFrom">The start index value.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = source.Skip((pageIndex - indexFrom) * pageSize)
                                    .Take(pageSize);

            var pagedList = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items
            };

            return pagedList;
        }
    }
}