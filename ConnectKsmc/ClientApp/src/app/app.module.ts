import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { NgxUiLoaderModule, SPINNER, POSITION, PB_DIRECTION } from 'ngx-ui-loader';

import { AppRoutingModule } from './app-routing.module';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { ToastModule } from 'primeng/toast';
import { ContextMenuModule } from 'primeng/contextmenu';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { KeyFilterModule } from 'primeng/keyfilter';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog'; ``
import { PasswordModule } from 'primeng/password';
import { CheckboxModule } from 'primeng/checkbox';
import { TriStateCheckboxModule } from 'primeng/tristatecheckbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { CalendarModule } from 'primeng/calendar';
import { TabViewModule } from 'primeng/tabview';
import { ChartModule } from 'primeng/chart';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { InputMaskModule } from 'primeng/inputmask';
import { PickListModule } from 'primeng/picklist';
import { AccordionModule } from 'primeng/accordion';
import { FieldsetModule } from 'primeng/fieldset';
import { FileUploadModule } from 'primeng/fileupload';
import { PanelModule } from 'primeng/panel';
import { CarouselModule } from 'primeng/carousel';

import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';
import { LoginService } from './services/login.service';
import { CommonService } from './services/common.service';
import { MenuService } from './services/menu.service';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { CommonComponent } from './components/common/common.component';
import { M35001Component } from './components/m35001/m35001.component';
import { T07026Component } from './components/transaction/t07026/t07026.component';
import { Q13001Component } from './components/query/q13001/q13001.component';
import { T30023Component } from './components/transaction/t30023/t30023.component';
import { T06201Component } from './components/transaction/t06201/t06201.component';
import { T06209Component } from './components/transaction/t06209/t06209.component';
import { T11013Component } from './components/transaction/t11013/t11013.component';
import { T07027Component } from './components/transaction/t07027/t07027.component';
import { T13115Component } from './components/transaction/t13115/t13115.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    CommonComponent,
    M35001Component,
    Q13001Component,
    T07026Component,
    T30023Component,
    T06201Component,
    T06209Component,
    T07027Component,
    T13115Component,
    T11013Component,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgIdleKeepaliveModule.forRoot(),
    NgxUiLoaderModule.forRoot({
      pbColor: 'red',
      fgsColor: 'red',
      bgsPosition: POSITION.bottomCenter,
      bgsSize: 40,
      bgsType: SPINNER.rectangleBounce,
      pbDirection: PB_DIRECTION.leftToRight,
      pbThickness: 5
    }),
    ScrollPanelModule,
    ToastModule,
    ContextMenuModule,
    InputTextModule,
    InputTextareaModule,
    DropdownModule,
    ButtonModule,
    TableModule,
    KeyFilterModule,
    ConfirmDialogModule,
    DialogModule,
    PasswordModule,
    CheckboxModule,
    TriStateCheckboxModule,
    RadioButtonModule,
    CalendarModule,
    TabViewModule,
    ChartModule,
    AutoCompleteModule,
    InputMaskModule,
    PickListModule,
    AccordionModule,
    FieldsetModule,
    FileUploadModule,
    PanelModule,
    CarouselModule
  ],
  providers: [
    MessageService,
    ConfirmationService,
    LoginService,
    CommonService,
    MenuService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
