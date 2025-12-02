import { ListService, PagedResultDto, LocalizationModule } from '@abp/ng.core';
import { Component, OnInit, inject } from '@angular/core';
import { BookDto, BookService, bookTypeOptions } from '../proxy/books';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CommonModule } from '@angular/common';
import { NgbModal, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap'; // add this line
// added this line
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-book',
  standalone: true,
  templateUrl: './book.html',
  styleUrls: ['./book.scss'],
  imports: [
    NgxDatatableModule,
    LocalizationModule,
    CommonModule,
    NgbModalModule,
    ReactiveFormsModule,
    NgbDatepickerModule
  ],
  providers: [ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter } // add this line
  ],
})
export class Book implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  form: FormGroup;
  bookTypes = bookTypeOptions;

  private readonly list = inject(ListService);
  private readonly fb = inject(FormBuilder);
  private readonly modalService = inject(NgbModal);
  private readonly bookService = inject(BookService);

  ngOnInit() {
    const streamCreator = (query) => this.bookService.getList(query);
    this.list.hookToQuery(streamCreator).subscribe(res => (this.book = res));
  }

  openCreateModal(content) {
    this.buildForm();
    this.modalService.open(content, { size: 'lg', backdrop: 'static' });
  }

  buildForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      type: [null, Validators.required],
      publishDate: [null, Validators.required],
      price: [null, Validators.required],
    });
  }

  save(modalRef) {
    if (this.form.invalid) return;

    this.bookService.create(this.form.value).subscribe(() => {
      modalRef.close();
      this.list.get(); // refresh list
    });
  }
}
