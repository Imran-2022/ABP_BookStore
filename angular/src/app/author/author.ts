import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

// Essential ABP Core Modules for Listing and Localization
import { ListService, PagedResultDto, LocalizationModule } from '@abp/ng.core';
import { PermissionService } from '@abp/ng.core'; // Still inject the service for later use

// External Modules
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap'; // Needed for the Actions dropdown shell

// Proxy & DTOs (Assuming this path is correct: '../proxy/authors')
import { AuthorDto, AuthorService } from '../proxy/authors';

// RxJS (Need Observable for later use, but not combineLatest yet)
import { Observable } from 'rxjs';


@Component({
  selector: 'app-author',
  standalone: true,
  imports: [
    CommonModule,
    LocalizationModule,
    NgxDatatableModule,
    NgbDropdownModule,
  ],
  templateUrl: './author.html',
  styleUrls: ['./author.scss'],
  providers: [
    ListService, // Crucial for ngx-datatable and data fetching
  ],
})
export class Author implements OnInit {
  author = { items: [], totalCount: 0 } as PagedResultDto<AuthorDto>;

  // INJECTIONS
  public readonly list = inject(ListService);
  private readonly authorService = inject(AuthorService);
  private readonly permissionService = inject(PermissionService);

  // Simple permissions setup (No complex logic yet)
  authorPolicyNames = {
    Create: 'BookStore.Authors.Create',
    Edit: 'BookStore.Authors.Edit',
    Delete: 'BookStore.Authors.Delete',
  };

  // Placeholder for the combined permission observable (will return false initially)
  hasAnyActionPermission$: Observable<boolean> = new Observable<boolean>();

  ngOnInit(): void {
    // Setup data listing stream
    // NOTE: If you still see the "expression not callable" error here, 
    // you MUST run `abp generate-proxy -t ng` and restart Angular.
    const authorStreamCreator = (query) => this.authorService.getList(query);
    this.list.hookToQuery(authorStreamCreator).subscribe((response) => {
      this.author = response;
    });
  }
}