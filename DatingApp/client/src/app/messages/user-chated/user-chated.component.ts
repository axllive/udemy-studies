import { Component, Input, OnInit } from '@angular/core';
import { chatedWith } from 'src/app/_models/chatedWith';
import { Message } from 'src/app/_models/message';

@Component({
  selector: 'app-user-chated',
  templateUrl: './user-chated.component.html',
  styleUrls: ['./user-chated.component.css']
})
export class UserChatedComponent implements OnInit{
  @Input() user: chatedWith | undefined;

  constructor() {}
  
  ngOnInit(): void {  }

}
