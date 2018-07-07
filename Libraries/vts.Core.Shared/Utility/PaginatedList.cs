﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace vts.Shared.Utility
{
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        public PaginatedList(IQueryable<T> collection, int skip, int take, int totalCount, bool alreadyPaged = false)
        {
            if (skip <= 0) skip = 1;
            PageIndex = skip - 1;
            PageSize = take;
            TotalItemCount = totalCount;
            PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;
            UnderlyingList = collection;
            if (alreadyPaged)
                AddRange(collection);
            else
                AddRange(collection.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public PaginatedList(List<T> collection, int skip, int take, int totalCount, bool alreadyPaged = false)
        {
            if (skip <= 0) skip = 1;
            PageIndex = skip - 1;
            PageSize = take;
            TotalItemCount = totalCount;
            PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;
            UnderlyingList = collection;
            AddRange(alreadyPaged ? collection : collection.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public int PageCount { get; private set; }
        public int TotalItemCount { get; private set; }
        public int PageIndex { get; private set; }
        public int PageNumber { get { return PageIndex + 1; } }
        public int PageSize { get; private set; }
        public bool HasPrevPage { get; private set; }
        public bool HasNextPage { get; private set; }
        public bool IsFirstPage { get; private set; }
        public bool IsLastPage { get; private set; }
        public IEnumerable<T> UnderlyingList { get; private set; }
    }
}