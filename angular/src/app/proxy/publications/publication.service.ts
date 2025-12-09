import type { CreatePublicationDto, GetPublicationListDto, PublicationDto, UpdatePublicationDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PublicationService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  create = (input: CreatePublicationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PublicationDto>({
      method: 'POST',
      url: '/api/app/publication',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/publication/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PublicationDto>({
      method: 'GET',
      url: `/api/app/publication/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPublicationListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PublicationDto>>({
      method: 'GET',
      url: '/api/app/publication',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePublicationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/publication/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}