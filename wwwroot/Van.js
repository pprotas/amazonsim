class Van extends THREE.Group {

    constructor() {
        super();

        this._loadState = LoadStates.NOT_LOADING;

        this.init();
    }

    get loadState() {
        return this._loadState;
    }

    init() {
         if(this._loadState != LoadStates.NOT_LOADING){
             return;
         }

         this._loadState = LoadStates.LOADING;

         var selfRef = this;
         loadOBJModel("models/", "Van.obj", "textures/", "Van.mtl", (mesh) => {
            mesh.scale.set(0.1, 0.1, 0.1);
            selfRef.add(mesh);

            addPointLight(selfRef, 0xffffff, -1, 2, 2, 2, 1);
            addPointLight(selfRef, 0xffffff, 1, 2, 2, 2, 1);

            addPointLight(selfRef, 0xff4751, -1, 2, -2, 2, 1);
            addPointLight(selfRef, 0xff4751, 1, 2, -2, 2, 1);

            selfRef._loadState = LoadStates.LOADED;
        });
    }
}