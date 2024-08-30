import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ColorSchemeComponent } from './components/color-scheme.component';
import { MAT_IMPORTS } from './mat.imports';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';
import { NavComponent } from './components/nav/nav.component';
import { ModelComponent } from './components/popup-model/popup-model.component';

const components: any[] = [ColorSchemeComponent, LayoutComponent];

@NgModule({
  declarations: [...components, NavComponent, ModelComponent],
  imports: [CommonModule, ...MAT_IMPORTS, RouterModule],
  exports: [...components],
})
export class SharedModule {}
