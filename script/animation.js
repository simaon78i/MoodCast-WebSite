/* Array of 4 poster URLs from Cloudinary */
    const posterUrls = [
        "https://res.cloudinary.com/ddja5g5wa/image/upload/v1766003766/action_b1qxmn.png",
        "https://res.cloudinary.com/ddja5g5wa/image/upload/v1766003765/comedy_vqb2qe.png",
        "https://res.cloudinary.com/ddja5g5wa/image/upload/v1766003765/romantic_hhbdvg.png",
        "https://res.cloudinary.com/ddja5g5wa/image/upload/v1766003765/drama_yj9tho.png"
    ];

    function createFloatingPosters() {
        const overlay = document.getElementById('loading-overlay');
        const posterCount = 12; // Increased to 12 for 4 posters

        for (let i = 0; i < posterCount; i++) {
            const img = document.createElement('img');
            img.src = posterUrls[i % posterUrls.length];
            img.className = 'floating-poster';

            const startTop = Math.random() * 100;
            const startLeft = Math.random() * 100;
            const moveX = (Math.random() - 0.5) * 800; 
            const moveY = (Math.random() - 0.5) * 800;

            img.style.top = startTop + '%';
            img.style.left = startLeft + '%';
            img.style.setProperty('--x', moveX + 'px');
            img.style.setProperty('--y', moveY + 'px');

            img.style.animation = `flyIn 6s infinite linear`;
            img.style.animationDelay = (Math.random() * 6) + 's';

            overlay.appendChild(img);
        }
    }

    window.addEventListener('load', function() {
        const loader = document.getElementById('loading-overlay');
        
        /* SessionStorage logic: Clear on app close, persist during navigation */
        if (sessionStorage.getItem('moodcast_init')) {
            if (loader) loader.remove();
        } else {
            createFloatingPosters();
            sessionStorage.setItem('moodcast_init', 'true');

            setTimeout(function() {
                if (loader) {
                    loader.style.transition = 'opacity 1.2s ease-out';
                    loader.style.opacity = '0';
                    setTimeout(() => loader.remove(), 1200);
                }
            }, 4500); 
        }
    });
