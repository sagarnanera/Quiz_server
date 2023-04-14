using AutoMapper;
using Quiz_server.DTOs;
using Quiz_server.Models;
using System.Runtime.CompilerServices;

namespace Quiz_server.Helper
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserName, opt => opt.NullSubstitute("NULL"))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserRegisterModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.NullSubstitute("NULL"))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserloginModel, UserDTO>()
                .ForMember(dest => dest.UserName, opt => opt.NullSubstitute("NULL"))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Quiz, QuizDTO>()
                .ForMember(dest => dest.QuizName, opt => opt.NullSubstitute("NULL"))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<QuizDTO, Quiz>()
                .ForMember(dest => dest.QuizName, opt => opt.NullSubstitute("NULL"))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Question, questionsResDTO>()
                .ForMember(dest => dest.QuestionString, opt => opt.NullSubstitute("NULL"))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<questionsReqDTO, Question>()
                .ForMember(dest => dest.QuestionString, opt => opt.NullSubstitute("NULL"))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<QuizResDTO, QuizResponse>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<scoreDTO,Score>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Score,scoreDTO>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
