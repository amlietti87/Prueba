import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { SortMeta } from 'primeng/components/common/sortmeta';
import { FilterMetadata } from 'primeng/components/common/filtermetadata';

export class PrimengDatatableHelper {
    predefinedRecordsCountPerPage = [5, 10, 25, 50, 100, 250, 500];

    defaultRecordsCountPerPage = 10;

    isResponsive = true;

    resizableColumns: false;

    totalRecordsCount = 0;

    records: any[];

    isLoading = false;

    showLoadingIndicator(): void {
        setTimeout(() => {
            this.isLoading = true;
        }, 0);
    }

    hideLoadingIndicator(): void {
        setTimeout(() => {
            this.isLoading = false;
        }, 0);
    }

    getSorting(dataTable: DataTable): string {
        let sorting;
        if (dataTable.sortField) {
            sorting = dataTable.sortField;
            if (dataTable.sortOrder === 1) {
                sorting += ' ASC';
            } else if (dataTable.sortOrder === -1) {
                sorting += ' DESC';
            }
        }

        return sorting;
    }



    getPageIndex(paginator: Paginator, event: LazyLoadEventData): number {
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages

        if (!event) {
            return (paginator.paginatorState != null ? (paginator.paginatorState.page || 0) : 0) + 1;
        }

        return (event.page || 0) + 1;
    }

    getPageSize(paginator: Paginator, event: LazyLoadEventData): number {
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
        if (!event || !event.rows) {
            return paginator.rows || this.defaultRecordsCountPerPage;
        }

        paginator.rows = event.rows;
        return event.rows;
    }
    getMaxResultCount(paginator: Paginator, event: LazyLoadEvent): number {
        if (paginator.rows) {
            return paginator.rows;
        }

        if (!event) {
            return 0;
        }

        return event.rows;
    }


    getSkipCount(paginator: Paginator, event: LazyLoadEvent): number {
        if (paginator.first) {
            return paginator.first;
        }

        if (!event) {
            return 0;
        }

        return event.first;
    }

    shouldResetPaging(event: LazyLoadEvent): boolean {
        if (!event /*|| event.sortField*/) { // if you want to reset after sorting, comment out parameter
            return true;
        }

        return false;
    }
}


export class LazyLoadEventData implements LazyLoadEvent {

    first?: number;
    rows?: number;
    sortField?: string;
    sortOrder?: number;
    multiSortMeta?: SortMeta[];
    filters?: {
        [s: string]: FilterMetadata;
    };
    globalFilter?: any;
    page: number;
}