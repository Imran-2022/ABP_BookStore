import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListService, PagedResultDto, LocalizationPipe, PermissionDirective } from '@abp/ng.core';
import { PublicationService, PublicationDto } from '../proxy/publications';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import {
  NgbDateNativeAdapter,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
} from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation, ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';

@Component({
  selector: 'app-publication',
  standalone: true,
  templateUrl: './publication.html',
  styleUrls: ['./publication.scss'],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    NgbDropdownModule,
    PageModule,
    LocalizationPipe,
    PermissionDirective,
    ThemeSharedModule,
  ],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class Publication implements OnInit {
  readonly list = inject(ListService);
  private publicationService = inject(PublicationService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);

  publication = { items: [], totalCount: 0 } as PagedResultDto<PublicationDto>;

  isModalOpen = false;
  form: FormGroup;
  selectedPublication = {} as PublicationDto;

  ngOnInit(): void {
    const publicationStream = query => this.publicationService.getList(query);

    this.list.hookToQuery(publicationStream).subscribe((response) => {
      this.publication = response;
    });
  }

  createPublication() {
    this.selectedPublication = {} as PublicationDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editPublication(id: string) {
    this.publicationService.get(id).subscribe((publication) => {
      this.selectedPublication = publication;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
  this.form = this.fb.group({
    name: [this.selectedPublication.name || '', Validators.required],
    location: [this.selectedPublication.location || '', Validators.required],
    website: [this.selectedPublication.website || '', Validators.required],
  });
}


  save() {
    if (this.form.invalid) return;

    if (this.selectedPublication.id) {
      this.publicationService
        .update(this.selectedPublication.id, this.form.value)
        .subscribe(() => {
          this.isModalOpen = false;
          this.form.reset();
          this.list.get();
        });
    } else {
      this.publicationService.create(this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.publicationService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
}
