// This file is for custom JavaScript functionality
document.addEventListener('DOMContentLoaded', function() {
    // Add any custom client-side behavior here
    console.log('Page loaded and ready');

    // Zelda: Breath of the Wild theme enhancements
    // Add sound effects for buttons (subtle)
    const buttons = document.querySelectorAll('.btn');
    buttons.forEach(button => {
        button.addEventListener('mouseenter', () => {
            addSheikahGlow(button);
        });
        
        button.addEventListener('mouseleave', () => {
            removeSheikahGlow(button);
        });
        
        button.addEventListener('click', () => {
            addClickEffect(button);
        });
    });
    
    // Add hover effects to cards
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', () => {
            card.style.transform = 'translateY(-3px)';
        });
        
        card.addEventListener('mouseleave', () => {
            card.style.transform = 'translateY(0)';
        });
    });
    
    // Add special effects to form elements with sheikah class
    const sheikahInputs = document.querySelectorAll('.sheikah-input');
    sheikahInputs.forEach(input => {
        input.addEventListener('focus', () => {
            input.classList.add('ancient-glow');
        });
        
        input.addEventListener('blur', () => {
            input.classList.remove('ancient-glow');
        });
    });
    
    // Add special effects to checkbox groups
    const checkboxes = document.querySelectorAll('.form-check-input');
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', () => {
            if (checkbox.checked) {
                // Add a subtle animation
                const label = checkbox.nextElementSibling;
                if (label) {
                    label.style.color = 'var(--primary-color)';
                    setTimeout(() => {
                        label.style.color = '';
                    }, 1000);
                }
            }
        });
    });
    
    console.log('Zelda theme enhancements activated');
});

// Helper functions
function addSheikahGlow(element) {
    element.style.boxShadow = '0 0 10px rgba(89, 193, 189, 0.5)';
}

function removeSheikahGlow(element) {
    element.style.boxShadow = '';
}

function addClickEffect(element) {
    // Create a ripple element
    const ripple = document.createElement('span');
    ripple.classList.add('btn-ripple');
    
    // Position the ripple
    ripple.style.position = 'absolute';
    ripple.style.top = '50%';
    ripple.style.left = '50%';
    ripple.style.transform = 'translate(-50%, -50%)';
    ripple.style.width = '0';
    ripple.style.height = '0';
    ripple.style.backgroundColor = 'rgba(255, 255, 255, 0.3)';
    ripple.style.borderRadius = '50%';
    ripple.style.pointerEvents = 'none';
    ripple.style.opacity = '1';
    ripple.style.transition = 'all 0.5s ease-out';
    
    // Save original position and overflow
    const originalPosition = element.style.position;
    if (originalPosition !== 'absolute' && originalPosition !== 'fixed' && originalPosition !== 'relative') {
        element.style.position = 'relative';
    }
    element.style.overflow = 'hidden';
    
    // Add the ripple
    element.appendChild(ripple);
    
    // Trigger the animation
    setTimeout(() => {
        ripple.style.width = '200px';
        ripple.style.height = '200px';
        ripple.style.opacity = '0';
    }, 10);
    
    // Clean up
    setTimeout(() => {
        element.removeChild(ripple);
        if (originalPosition !== 'absolute' && originalPosition !== 'fixed' && originalPosition !== 'relative') {
            element.style.position = originalPosition;
        }
    }, 500);
}
