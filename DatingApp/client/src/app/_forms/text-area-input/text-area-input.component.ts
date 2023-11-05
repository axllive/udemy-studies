import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-area-input',
  templateUrl: './text-area-input.component.html',
  styleUrls: ['./text-area-input.component.css']
})
export class TextAreaInputComponent implements ControlValueAccessor{
  @Input() label = '';
  @Input() type = 'text';
  @Input() rows = 3;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
   }

  writeValue(obj: any): void { }
  
  registerOnChange(fn: any): void { }

  registerOnTouched(fn: any): void { }

  get control(): FormControl{
    return this.ngControl.control as FormControl
  }

}
