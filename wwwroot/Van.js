class Van extends THREE.Group {

    /**
     * Constructor voor het aanmaken van een Van
     * [Loadstate op not loading]
     */
    constructor() {
        super();

        this._loadState = LoadStates.NOT_LOADING;

        this.init();
    }

    /**
     * Geeft de loadState van de Van terug
     */
    get loadState() {
        return this._loadState;
    }

    /**
     * Textures en pointlights worden toegevoegd aan de Van
     */
    init() {
         if(this._loadState != LoadStates.NOT_LOADING){
             return;
         }

         this._loadState = LoadStates.LOADING;

         var selfRef = this;
         loadOBJModel("models/", "Van.obj", "textures/", "Van.mtl", (mesh) => {
            mesh.scale.set(0.1, 0.1, 0.1);
            mesh.castShadow = true;
            selfRef.add(mesh);

            addPointLight(selfRef, 0xffffff, -1, 1.5, 2.6, 2, 5);
            addPointLight(selfRef, 0xffffff, 1, 1.5, 2.6, 2, 5);

            addPointLight(selfRef, 0xff4751, -1, 1.5, -2.7, 2, 7);
            addPointLight(selfRef, 0xff4751, 1, 1.5, -2.7, 2, 7);

            selfRef._loadState = LoadStates.LOADED;
        });
    }
}