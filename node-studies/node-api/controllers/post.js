const Post = require('../models/post');

exports.getLindas = (req, res) => {
    res.json({
        posts: [
            {title: 'First post'},
            {title: 'Second post'}
        ]
    });
};

exports.createPost = (req, res) => {
    const post = new Post(req.body);
    /*console.log("CREATING POST: ", req.body); */
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
    post.save()
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
    } );
};