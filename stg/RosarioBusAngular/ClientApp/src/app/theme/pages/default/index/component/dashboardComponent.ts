export interface IDashboardBaseComponent {
    data: any;
}

import { Component, Input } from '@angular/core';



@Component({
    template: `
    <div class="job-ad">
      <h4>{{data.headline}}  Dashboard1</h4>

      {{data.body}}
    </div>
  `
})
export class Dashboard1Component implements IDashboardBaseComponent {
    @Input() data: any;

}


