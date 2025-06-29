// Admin JavaScript Functionality

$(document).ready(function() {
    // Sidebar toggle functionality
    $('#sidebarToggle, #mobileSidebarToggle').click(function() {
        $('#adminSidebar').toggleClass('show');
    });

    // Close sidebar when clicking outside on mobile
    $(document).click(function(e) {
        if ($(window).width() < 992) {
            if (!$(e.target).closest('#adminSidebar, #mobileSidebarToggle').length) {
                $('#adminSidebar').removeClass('show');
            }
        }
    });

    // Navigation group toggle
    $('.admin-nav-group-toggle').click(function(e) {
        e.preventDefault();
        var target = $(this).data('target');
        var $submenu = $(target);
        var $navItem = $(this).closest('.admin-nav-item');
        var $toggle = $(this);
        
        // Use Bootstrap collapse but handle states manually
        $submenu.collapse('toggle');
        
        // Handle events for line animation
        $submenu.off('shown.bs.collapse hidden.bs.collapse'); // Remove previous handlers
        
        $submenu.on('shown.bs.collapse', function() {
            $navItem.addClass('expanded');
            $toggle.attr('aria-expanded', 'true');
        });
        
        $submenu.on('hidden.bs.collapse', function() {
            $navItem.removeClass('expanded');
            $toggle.attr('aria-expanded', 'false');
        });
        
        // Immediate state check for faster response
        setTimeout(function() {
            if ($submenu.hasClass('show')) {
                $navItem.addClass('expanded');
                $toggle.attr('aria-expanded', 'true');
            } else {
                $navItem.removeClass('expanded');
                $toggle.attr('aria-expanded', 'false');
            }
        }, 10);
    });

    // Auto-expand active menu groups
    $('.admin-nav-sublink.active').each(function() {
        var $submenu = $(this).closest('.admin-nav-submenu');
        var $navItem = $submenu.closest('.admin-nav-item');
        var $toggle = $navItem.find('.admin-nav-group-toggle');
        
        $submenu.addClass('show');
        $navItem.addClass('expanded');
        $toggle.attr('aria-expanded', 'true');
    });

    // Confirm delete actions
    $('.btn-delete, .admin-btn-danger[href*="Delete"]').click(function(e) {
        if (!confirm('Bạn có chắc chắn muốn xóa không? Hành động này không thể hoàn tác.')) {
            e.preventDefault();
        }
    });

    // Auto-hide alerts after 5 seconds
    $('.alert').each(function() {
        var $alert = $(this);
        setTimeout(function() {
            $alert.fadeOut();
        }, 5000);
    });

    // Search functionality
    $('.admin-search').on('input', function() {
        var value = $(this).val().toLowerCase();
        var target = $(this).data('target');
        
        $(target + ' tr').filter(function() {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // Toggle status buttons
    $('.btn-toggle-status').click(function(e) {
        e.preventDefault();
        var $btn = $(this);
        var url = $btn.attr('href');
        
        $.post(url, function() {
            location.reload();
        }).fail(function() {
            alert('Có lỗi xảy ra. Vui lòng thử lại.');
        });
    });

    // Image preview for file inputs
    $('input[type="file"][accept*="image"]').change(function() {
        var input = this;
        var $preview = $(input).siblings('.image-preview');
        
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            
            reader.onload = function(e) {
                if ($preview.length === 0) {
                    $preview = $('<div class="image-preview mt-2"><img class="img-thumbnail" style="max-width: 200px;"></div>');
                    $(input).after($preview);
                }
                $preview.find('img').attr('src', e.target.result);
            };
            
            reader.readAsDataURL(input.files[0]);
        }
    });

    // Form validation enhancement
    $('form').submit(function() {
        var $form = $(this);
        var $submitBtn = $form.find('button[type="submit"]');
        
        // Disable submit button to prevent double submission
        $submitBtn.prop('disabled', true);
        
        // Re-enable after 3 seconds (in case of validation errors)
        setTimeout(function() {
            $submitBtn.prop('disabled', false);
        }, 3000);
    });

    // Tooltip initialization
    $('[data-bs-toggle="tooltip"]').tooltip();

    // Popover initialization  
    $('[data-bs-toggle="popover"]').popover();

    // Number formatting
    $('.format-number').each(function() {
        var num = parseFloat($(this).text());
        if (!isNaN(num)) {
            $(this).text(num.toLocaleString('vi-VN'));
        }
    });

    // Currency formatting
    $('.format-currency').each(function() {
        var num = parseFloat($(this).text());
        if (!isNaN(num)) {
            $(this).text(num.toLocaleString('vi-VN') + ' ₫');
        }
    });

    // Date formatting
    $('.format-date').each(function() {
        var date = new Date($(this).text());
        if (!isNaN(date.getTime())) {
            $(this).text(date.toLocaleDateString('vi-VN'));
        }
    });

    // Chart.js default configuration
    if (typeof Chart !== 'undefined') {
        Chart.defaults.font.family = "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif";
        Chart.defaults.color = '#6c757d';
    }
});

// Utility functions
window.AdminUtils = {
    // Show loading state
    showLoading: function($element) {
        $element.prop('disabled', true);
        var originalText = $element.text();
        $element.data('original-text', originalText);
        $element.html('<i class="fas fa-spinner fa-spin"></i> Đang xử lý...');
    },
    
    // Hide loading state
    hideLoading: function($element) {
        $element.prop('disabled', false);
        var originalText = $element.data('original-text');
        if (originalText) {
            $element.text(originalText);
        }
    },
    
    // Show toast notification
    showToast: function(message, type = 'success') {
        var alertClass = 'alert-' + type;
        var icon = type === 'success' ? 'check-circle' : 
                  type === 'error' ? 'exclamation-circle' : 
                  type === 'warning' ? 'exclamation-triangle' : 'info-circle';
        
        var $toast = $(`
            <div class="alert ${alertClass} alert-dismissible fade show position-fixed" 
                 style="top: 20px; right: 20px; z-index: 9999; min-width: 300px;">
                <i class="fas fa-${icon} me-2"></i>
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `);
        
        $('body').append($toast);
        
        setTimeout(function() {
            $toast.fadeOut(function() {
                $(this).remove();
            });
        }, 5000);
    },
    
    // Format currency
    formatCurrency: function(amount) {
        return parseFloat(amount).toLocaleString('vi-VN') + ' ₫';
    },
    
    // Format number
    formatNumber: function(number) {
        return parseFloat(number).toLocaleString('vi-VN');
    },
    
    // Format date
    formatDate: function(date) {
        return new Date(date).toLocaleDateString('vi-VN');
    }
}; 