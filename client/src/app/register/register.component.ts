import {
  Component,
  EventEmitter,
  inject,
  input,
  Input,
  output,
  Output,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  // usersFromHomeComponent = input.required<any>();
  /*@Output()*/ cancelRegister = output<boolean>() /*new EventEmitter()*/;
  private toastr = inject(ToastrService);
  model: any = {};

  register() {
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => this.toastr.error(error.error),
      complete: () => this.toastr.success("Registration done successfuly"),
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
