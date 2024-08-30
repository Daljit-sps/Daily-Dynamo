import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable, of, switchMap } from 'rxjs';
import { DSRService } from '../../services/dsr.service';
import { TDSRResponse } from '../../types/dsr-types';

@Component({
  selector: 'app-dsr-details',
  templateUrl: './dsr-details.component.html',
  styleUrls: ['./dsr-details.component.scss'],
})
export class DsrDetailsComponent implements OnInit {
  router: ActivatedRoute = inject(ActivatedRoute);
  service: DSRService = inject(DSRService);
  dsr: Observable<TDSRResponse> = of();

  ngOnInit(): void {
    this.dsr = this.router.params
      .pipe(map(params => params['id']))
      .pipe(switchMap(id => this.service.getDsr(id)))
      .pipe(map(response => response.data));
  }
}
