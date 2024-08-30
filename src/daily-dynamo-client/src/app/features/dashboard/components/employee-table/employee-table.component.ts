import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { TDSRStatusList, TDSRStatusTable, TUserDSRList } from '../../types/dashboard-table.type';
import { Store } from '@ngrx/store';
import { TAppState } from 'src/app/types/app-state.type';
import { selectUserId } from 'src/app/store/auth.selector';
import { Observable, map, of, take } from 'rxjs';
import { DashboardService } from '../../services/dashboard.service';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { DSRService } from '../../services/dsr.service';
import { ViewSelectedDSRModelComponent } from '../view-dsr-model/view-dsr-model.component';
import { MatDialog } from '@angular/material/dialog';
import { TDSRResponse } from '../../types/dsr-types';

@Component({
  selector: 'app-employee-table',
  templateUrl: './employee-table.component.html',
  styleUrls: ['./employee-table.component.scss'],
})
export class EmployeeTableComponent implements OnInit {
  router: Router = inject(Router);
  store: Store<TAppState> = inject(Store<TAppState>); 
  service: DashboardService = inject(DashboardService);
  dsrService: DSRService = inject(DSRService);
  dialog: MatDialog = inject(MatDialog);

  userId: string | undefined;
  dsrStatusList: MatTableDataSource<TDSRStatusList> = new MatTableDataSource<TDSRStatusList>([]);
  userDSRList: MatTableDataSource<TUserDSRList> = new MatTableDataSource<TUserDSRList>([]);
  showDsrStatusTable = false;
  showUserDsrTable = false;
  isDataNull = false;

  totalItems = 100; 
  pageSize = 3; 
  pageSizeOptions = [3, 6, 9, 12]; 

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.store.select(selectUserId)
    .pipe(take(1))
        .subscribe(uId => {
        if (uId) {
          this.userId = uId;
          this.getTableData(1, 3);
        }
        });
     
     this.dsrStatusList.paginator = this.paginator;
     this.userDSRList.paginator = this.paginator;
  }

  getTableData(page: number, offset: number): void {
    this.service.getData(page, offset).pipe(
      map((res: any) => {
        console.log(res);
        if(res.data == null)
          this.isDataNull = true;
        else
        this.isDataNull = false;

        return {
          totalItems: res.data.totalItems,
          data: res.data.data,
        };
      }),
      map((result: { totalItems: number, data: TDSRStatusTable[] }) => {
        if(!this.isDataNull){
          this.dsrStatusList = new MatTableDataSource(
            result.data.filter(item => item.name !== null && item.status !== null)
              .map(({ dsrId, name, status }) => ({ dsrId, name, status }))
          );
      
          this.userDSRList = new MatTableDataSource(
            result.data.filter(item => item.name === null || item.status === null)
              .map(({ dsrId, reportDate, taskAccomplished, nextDayPlan }) => ({ dsrId, reportDate, taskAccomplished, nextDayPlan }))
          );
      
          this.showDsrStatusTable = this.dsrStatusList.data.length > 0;
          this.showUserDsrTable = this.userDSRList.data.length > 0;
          console.log(this.dsrStatusList, this.userDSRList);
      
          this.totalItems= result.totalItems; 
        }
        
      })
    ).subscribe({
      next: () => {},
      error: (error: any) => {},
    });
    
    
  }

  onPageChange(event: any): void {
    const pageIndex = event.pageIndex;
    const pageSize = event.pageSize;
    const offset = pageIndex * pageSize;
  
    // Call your data loading method with the updated page and offset values
    this.getTableData(pageIndex + 1, pageSize); 
  }

  truncateText(text: string): string {
    const words = text.split(' ');
    if (words.length > 4) {
      return words.slice(0, 4).join(' ') + '...';
    }
    return text;
  }
  
  viewDSR(selectedDSR: any): void{
    var dsrId = selectedDSR.dsrId;
    this.dsrService.getDsr(dsrId)
    .pipe(
      map((res: any) => {
        console.log(res);
        return res;
      }),
    )  
    .subscribe({
      next: (res) => {this.openDSRDialog(res.data)},
      error: () => {},
    });
  }
  openDSRDialog(data: TDSRResponse): void {
    const dialogRef = this.dialog.open(ViewSelectedDSRModelComponent, {
      width: '400px',
      data: { selectedDSR: data }
    });
  
    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }
  
}

