//Javascript enum
const LoadStates =
    Object.freeze(
        {
            "NOT_LOADING": 1,
            "LOADING": 2,
            "LOADED": 3
        });

class Robot extends THREE.Group {

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
         loadOBJModel("models/", "DomesticRobot_1293.obj", "textures/", "DomesticRobot_1293.mtl", (mesh) => {
            mesh.scale.set(0.05, 0.05, 0.05);
            selfRef.add(mesh);

            addPointLight(selfRef, 0xff4751, 0, 0.75, 0, 1, 1);

            selfRef._loadState = LoadStates.LOADED;
        });
    }
}

function addPointLight(object, color, x, y, z, intensity, distance) {
    var light = new THREE.PointLight(color, intensity, distance);
    light.position.set(x, y, z);
    object.add(light);
    var pointLightHelper2 = new THREE.PointLightHelper(light, 1, 0xff0000);
    object.add(pointLightHelper2);
}

function loadOBJModel(modelPath, modelName, texturePath, textureName, onload) {
    new THREE.MTLLoader()
        .setPath(texturePath)
        .load(textureName, function (materials) {
            materials.preload();

            new THREE.OBJLoader()
                .setPath(modelPath)
                .setMaterials(materials)
                .load(modelName, function (object) {
                    onload(object);
                }, function () { }, function (e) { console.log("Error loading model"); console.log(e); });
        });
}