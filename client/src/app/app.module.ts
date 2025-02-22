import { ClipboardModule } from '@angular/cdk/clipboard';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { curray } from 'curray';
import { FileSizePipe, NgxFilesizeModule } from 'ngx-filesize';
import { AddNewTorrentComponent } from './add-new-torrent/add-new-torrent.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './auth.interceptor';
import { DownloadStatusPipe } from './download-status.pipe';
import { LoginComponent } from './login/login.component';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SettingsComponent } from './settings/settings.component';
import { SetupComponent } from './setup/setup.component';
import { TorrentStatusPipe } from './torrent-status.pipe';
import { TorrentTableComponent } from './torrent-table/torrent-table.component';
import { TorrentComponent } from './torrent/torrent.component';
import { DecodeURIPipe } from './decode-uri.pipe';
import { ProfileComponent } from './profile/profile.component';

curray();

@NgModule({
  declarations: [
    AppComponent,
    MainLayoutComponent,
    NavbarComponent,
    AddNewTorrentComponent,
    TorrentTableComponent,
    SettingsComponent,
    TorrentStatusPipe,
    DownloadStatusPipe,
    LoginComponent,
    SetupComponent,
    TorrentComponent,
    DecodeURIPipe,
    ProfileComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    NgxFilesizeModule,
    FlexLayoutModule,
    ClipboardModule,
  ],
  providers: [FileSizePipe, { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent],
})
export class AppModule {}
