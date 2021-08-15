import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ToastrModule} from 'ngx-toastr';
import { NgxGalleryModule } from '@kolkov/ngx-gallery'
import { FileUploadModule } from 'ng2-file-upload';
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker'

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ToastrModule.forRoot({
      positionClass: "toast-bottom-right"
    }),
    NgxGalleryModule,
    FileUploadModule,
    BsDatepickerModule.forRoot()
    //BsDropdownModule.forRoot()
  ],
  exports: [
     ToastrModule,
     NgxGalleryModule,
     FileUploadModule,
     BsDatepickerModule
  ]
})
export class SharedModule { }
