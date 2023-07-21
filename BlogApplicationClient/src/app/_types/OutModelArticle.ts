import { OutModelComment } from './OutModelComment';
export interface OutModelArticle {
    owner: string;
    id: string;
    title: string;
    text: string;
    images: string[];
    template: string;
    comments: OutModelComment[];
  }
  