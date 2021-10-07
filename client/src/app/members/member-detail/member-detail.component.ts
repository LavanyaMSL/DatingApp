import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_modules/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
 member:Member;
 galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(private memberService:MembersService,private route:ActivatedRoute) { 

  }

  ngOnInit(): void {
    this.loadMember();
    this.galleryOptions = [
      {
        width: '600px',
        height: '400px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview:false
      }
    ]

   
    
  }
  getImages():NgxGalleryImage[]{
    const imageUrl=[];
    for(const photo of this.member.photos)
    {
      imageUrl.push({
        small:photo?.url,
        medium:photo?.url,
        large:photo?.url
      })
    }
    return imageUrl;
  }

  loadMember(){
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(user=>{
     this.member=user;
     this.galleryImages = this.getImages();
    })
  }
}
