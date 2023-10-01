const Post = require('../models/post');

exports.getEmpty = (req, res) => {
    res.json({
        posts: [
            {title: 'First post'},
            {title: 'Second post'}
        ]
    });
};

exports.createPost = (req, res) => {
    const post = new Post(req.body);

    /**OLD WAY
     * callbacks em Model.save foram depreciados
     *post.save( (err, result) => {
        if(err) {
            return res.status(400).json({
            error: err
            });
        }
        res.status(200).json({
            post: result
        });
        
    });
     */

    /**non external validation
    * post.save()
    .then(
        (result) => (
            res.status(200).json({
                post: result
            })
        )
    )
    .catch( (err, result) => {
        if(err) {
            return res.status(400).json({
            error: err
            });
        }
    } ); */

    post.save()
        .then(
            result => {
                res.status(200).json(
                    {
                        post: result
                    }
                );
            });
    
};

exports.getAll = (req, res) => {
    const posts = Post.find().select( "_id title body" ) //custom returns
        .then((posts) => {
            //se ambos os retornos são iguais: (posts: posts)
            //res.status(200).json({posts : posts});
            //pode-se resumir em uma única chamada
            res.status(200).json({posts});
        })
        .catch( err => console.log(err) );
};

exports.getOne = (req, res)=> {
    const posts = Post.findById(req.body).select( "_id title body" ) //custom returns
        .then((posts) => {
            res.status(200).json({posts});
        })
        .catch( err => console.log(err) );
};