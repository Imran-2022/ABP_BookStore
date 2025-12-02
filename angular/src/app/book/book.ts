import { ListService, PagedResultDto, LocalizationModule } from '@abp/ng.core';
import { Component, OnInit, inject } from '@angular/core';
import { BookDto, BookService, bookTypeOptions } from '../proxy/books';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CommonModule } from '@angular/common';
import { NgbDropdownModule, NgbModal, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';

// Add imports for ConfirmationService
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

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
    NgbDatepickerModule,
    NgbDropdownModule
  ],
  providers: [
    ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter },
    // You need to ensure ConfirmationService is available, often provided in a core module,
    // but we inject it here.
  ],
})
export class Book implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  // Declare selectedBook to hold the current book data for form
  selectedBook = {} as BookDto;

  form: FormGroup;
  bookTypes = bookTypeOptions;

  private readonly list = inject(ListService);
  private readonly fb = inject(FormBuilder);
  private readonly modalService = inject(NgbModal);
  private readonly bookService = inject(BookService);
  // Inject ConfirmationService
  private readonly confirmation = inject(ConfirmationService);

  ngOnInit() {
    const streamCreator = (query) => this.bookService.getList(query);
    this.list.hookToQuery(streamCreator).subscribe(res => (this.book = res));
  }

  // Modified: Sets selectedBook to an empty object before opening the modal
  openCreateModal(content) {
    this.selectedBook = {} as BookDto; // Reset selectedBook for new creation
    this.buildForm();
    this.modalService.open(content, { size: 'lg', backdrop: 'static' });
  }

  // New: Fetches book data, sets selectedBook, and opens the modal
  editBook(id: string, content) {
    this.bookService.get(id).subscribe((book) => {
      this.selectedBook = book;
      this.buildForm();
      this.modalService.open(content, { size: 'lg', backdrop: 'static' });
    });
  }

  // Modified: Initializes the form using data from selectedBook
  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedBook.name || '', Validators.required],
      type: [this.selectedBook.type || null, Validators.required],
      publishDate: [
        this.selectedBook.publishDate ? new Date(this.selectedBook.publishDate) : null,
        Validators.required,
      ],
      price: [this.selectedBook.price || null, Validators.required],
    });
  }

  // Modified: Handles both create (if no selectedBook.id) and update (if selectedBook.id exists)
  save(modalRef) {
    if (this.form.invalid) return;

    const request = this.selectedBook.id
      ? this.bookService.update(this.selectedBook.id, this.form.value)
      : this.bookService.create(this.form.value);

    request.subscribe(() => {
      modalRef.close();
      this.list.get(); // refresh list
    });
  }

  // New: Deletes a book after showing a confirmation popup
  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.bookService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
}