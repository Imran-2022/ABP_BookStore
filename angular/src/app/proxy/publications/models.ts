import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreatePublicationDto {
  name: string;
  location?: string;
  website?: string;
}

export interface GetPublicationListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface PublicationDto extends EntityDto<string> {
  name?: string;
  location?: string;
  website?: string;
}

export interface UpdatePublicationDto {
  name: string;
  location?: string;
  website?: string;
}
