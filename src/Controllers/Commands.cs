using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Newtonsoft.Json;

namespace Controllers {

    public abstract class Command {

        private string type;
        private Object parameters;

        public Command(string type, Object parameters) {
            this.type = type;
            this.parameters = parameters;
        }

        public string ToJson() {
            return JsonConvert.SerializeObject(new {
                command = type,
                parameters = parameters
            });
        }
    }

    public abstract class Model3DCommand : Command {

        public Model3DCommand(string type, Model3D parameters) : base(type, parameters) {
        }
    }

    public class UpdateModel3DCommand : Model3DCommand {
        
        public UpdateModel3DCommand(Model3D parameters) : base("update", parameters) {
        }
    }

    public class SendGraph : Command{
        public SendGraph(Graph parameters) : base("graph", parameters) {            
        }
    }
}