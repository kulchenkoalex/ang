import { Component } from '@angular/core';

@Component({
  template: `
  <div ace-editor
       [(text)]="text" // possible two way binding (thx ChrisProlls)
       [mode]="'sql'" //string or object (thx ckiffel)
       [theme]="'eclipse'"
       [options]="options"
       [readOnly]="false"
       [autoUpdateContent]="true" //change content when [text] change
       [durationBeforeCallback]="1000" //wait 1s before callback 'textChanged' sends new value
       (textChanged)="onChange($event)"
       style="min-height: 200px; width:100%; overflow: auto;"></div>
  `
})
/** my component*/
export class MyComponent {
    /** my ctor */
    //constructor() {

    //}
  text: string = "";
  options: any = { maxLines: 1000, printMargin: false };

  onChange(code) {
    console.log("new code", code);
  }
}
