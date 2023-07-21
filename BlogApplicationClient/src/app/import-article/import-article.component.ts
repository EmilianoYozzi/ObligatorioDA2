import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ArticleService } from '../article.service';
import { OutModelImport } from '../_types/OutModelImport';
import { InModelImport } from '../_types/InModelImport';
import { InModelParameter } from '../_types/InModelParameter';
import { OutModelArticle } from '../_types/OutModelArticle';
import { ToastrService } from 'ngx-toastr';



@Component({
  selector: 'app-import-article',
  templateUrl: './import-article.component.html',
  styleUrls: ['./import-article.component.css']
})
export class ImportArticleComponent implements OnInit {
  importers: OutModelImport[] = [];
  selectedImporter: OutModelImport | null = null;
  parametersForm: FormGroup;
  importResult: OutModelArticle[] | null = null; // Add this line

  constructor(
    private toastr: ToastrService,
    private articleService: ArticleService,
    private formBuilder: FormBuilder
  ) {
    this.parametersForm = this.formBuilder.group({});
  }

  ngOnInit(): void {
    this.articleService.getImporters().subscribe(
      importers => {
        console.log('Importers', importers);
        this.importers = importers;
  
        // Check if there are any importers
        if (this.importers.length > 0) {
          // Select the first one
          this.selectedImporter = this.importers[0];
  
          // Initialize form controls for this importer
          const group: any = {};
          this.selectedImporter.parameters.forEach(param => {
            group[param.name] = ['', param.necessary ? Validators.required : null];
          });
          this.parametersForm = this.formBuilder.group(group);
        }
      },
      error => console.error(error)
    );
  }
  
  

  onImporterChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    if (target) {
      this.selectedImporter = this.importers.find(importer => importer.name === target.value) || null;
      if (this.selectedImporter) {
        const group: any = {};
        this.selectedImporter.parameters.forEach(param => {
          group[param.name] = ['', param.necessary ? Validators.required : null];
        });
        this.parametersForm = this.formBuilder.group(group);
      }
    }
  }
  
  

  importArticles(): void {
    if (this.selectedImporter && this.parametersForm.valid) {
      const parameters: InModelParameter[] = this.selectedImporter.parameters.map(param => ({
        ParameterType: param.parameterType,
        Name: param.name,
        Value: this.parametersForm.get(param.name)?.value
      }));
      const importer: InModelImport = {
        Name: this.selectedImporter.name,
        Parameters: parameters
      };
      console.log('importer', importer);
      this.articleService.importArticles(importer).subscribe(
        response => {
          console.log('Import success!', response);
          this.toastr.success('Imported successfully!', 'Success');
        },
        error => {
          console.error(error);
          this.toastr.error('Import failed. Please try again.', 'Error');
        }
      );
    }
  }
  
  
  
  
}