const express = require('express');
const postController = require('../controllers/post');
const validator = require('../validators');
const router = express.Router();

router.get('/', postController.getLindas);
router.get('/all', postController.getAll);
router.get('/get/', postController.getOne);

router.post('/post', validator.createPostValidator, postController.createPost);

module.exports = router;