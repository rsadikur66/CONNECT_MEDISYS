import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { M35001Component } from './components/m35001/m35001.component';
import { Q13001Component } from './components/query/q13001/q13001.component';
import { T07026Component } from './components/transaction/t07026/t07026.component';
import { T30023Component } from './components/transaction/t30023/t30023.component';
import { T06201Component } from './components/transaction/t06201/t06201.component';
import { T07027Component } from './components/transaction/t07027/t07027.component';
import { T13115Component } from './components/transaction/t13115/t13115.component';
import { T06209Component } from './components/transaction/t06209/t06209.component';
import { T11013Component } from './components/transaction/t11013/t11013.component';

const routes: Routes = [
  { path: '', component: LoginComponent, pathMatch: 'full' },
  { path: 'Login', component: LoginComponent },
  { path: 'M35001', component: M35001Component },
  { path: 'Transaction/T07026', component: T07026Component },
  { path: 'Transaction/T30023/:patNo/:docCode/:apptNo/:clinicCode', component: T30023Component },
  { path: 'Transaction/T13115/:patNo/:patType/:visitNo/:icd10', component: T13115Component },
  { path: 'Query/Q13001/:patNo', component: Q13001Component },
  { path: 'Transaction/T11013/:patNo/:patType/:clinicCode/:hospCode', component: T11013Component },
  { path: 'Transaction/T06201/:patNo', component: T06201Component },
  { path: 'Transaction/T07027/:patNo', component: T07027Component },
  { path: 'Transaction/T06209/:patNo', component: T06209Component },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
