import { Component, ViewChild, AfterViewInit } from '@angular/core';
import 'brace';
import '../ace-builds/src-noconflict/mode-kulniy.js';

//import '../ace-builds/src-noconflict/ace.js';
//import '../ace-build/src-noconflict/ext-language_tools.js';
import 'brace/mode/html';
import 'brace/mode/javascript';
import 'brace/theme/eclipse';


@Component({
    selector: 'app-my',
    templateUrl: './my.component.html',
    styleUrls: ['./my.component.css']
})
/** my component*/
export class MyComponent implements AfterViewInit{
  name = 'Angular 6';
  @ViewChild('editor') editor;

  ngAfterViewInit() {


  }
}
