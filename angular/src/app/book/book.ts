import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { BookDto, BookService } from '../proxy/books';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { LocalizationModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book',
  imports: [NgxDatatableModule,LocalizationModule,CommonModule],
  templateUrl: './book.html',
  styleUrl: './book.scss',
  providers: [ListService]

})
export class Book  implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  public readonly list = inject(ListService);
  private readonly bookService = inject(BookService);

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getList(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }
}