import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginResponseModel } from '../../../models/login.response.model';
import { AuthService } from '../../../services/auth.service';
import { HttpService } from '../../../services/http.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(
    private router: Router,
    public auth:AuthService,
    public http:HttpService
  ) { }

  logout() {
    localStorage.clear();
    this.router.navigateByUrl("/login");
  }
  changeCompany(){
    this.http.post<LoginResponseModel>("Auth/ChangeCompany", {companyId: this.auth.user.companyId},res=> {
      localStorage.clear();
      localStorage.setItem("token", res.token);

      document.location.reload();
    });
  }
}
