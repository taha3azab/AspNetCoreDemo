export interface PagedList<T> {
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  indexFrom: number;
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  items: T[];
}
