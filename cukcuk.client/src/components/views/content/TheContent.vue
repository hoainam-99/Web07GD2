<template>
    <div class="content">
        <div class="content-header">
            <div class="content-header__left">
                Nguyên vật liệu
            </div>
            <div class="content-header__right">
                <button class="btn btn-feedback">
                    <i class="fa-solid fa-bullhorn"></i>
                    Phản hồi
                </button>
            </div>
        </div>
        <div class="content-body">
            <div class="content-toolbar">
                <button class="btn btn-add">
                    <i class="fa-solid fa-file-circle-plus"></i>
                    Thêm
                </button>
                <button class="btn btn-replication">
                    <i class="fa-regular fa-copy"></i>
                    Nhân bản
                </button>
                <button class="btn btn-edit">
                    <i class="fa-regular fa-pen-to-square"></i>
                    Sửa
                </button>
                <button class="btn btn-delete">
                    <i class="fa-solid fa-x"></i>
                    Xóa
                </button>
                <button class="btn btn-refresh">
                    <i class="fa-solid fa-arrows-rotate"></i>
                    Nạp
                </button>
            </div>
            <div class="content-table"></div>
            <div class="content-navigation"></div>
        </div>
    </div>
</template>

<script>
export default {

}
</script>

<style>
 @import url(./content.css);
</style>