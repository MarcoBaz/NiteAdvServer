export class Response {
    Operation: string;
    Error: string;
    Data:any;
    
       /* Constructor
       *
       * @param contact
       */
      constructor(operation) {
          {
          this.Operation = operation;
          this.Error =  '';
          this.Data = null;
          }
      }
      
  }